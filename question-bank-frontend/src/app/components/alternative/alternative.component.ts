import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AddAlternative, Alternative, UpdateAlternative } from 'src/app/models/alternative.model';
import { Question } from 'src/app/models/question.model';
import { AlternativeService } from 'src/app/services/alternative.service';
import { QuestionService } from 'src/app/services/question.service';

@Component({
  selector: 'app-alternative',
  templateUrl: './alternative.component.html',
  styleUrls: ['./alternative.component.css']
})
export class AlternativeComponent implements OnInit {

  formGroup!: FormGroup;
  isEditing: boolean = false;
  alternatives: Alternative[] = [];
  alternativeIdEdited!: number;
  questions: Question[] = [];
  questionStatements: { [key: number]: string } = {};
  errorMessage: string | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private alternativeService: AlternativeService,
    private questionService: QuestionService
  ) { }

  ngOnInit(): void {
    this.initForm();
    this.loadAlternatives();
    this.loadQuestions();
  }

  initForm(): void {
    this.formGroup = this.formBuilder.group({
      text: ['', [Validators.required]],
      isCorrect: ['', [Validators.required]],
      questionId: ['', [Validators.required]]
    });
  }

  onSubmitForm(): void {
    if (this.formGroup.valid) {
      const text = this.formGroup.get('text')?.value;
      const isCorrect = this.formGroup.get('isCorrect')?.value;
      const questionId = this.formGroup.get('questionId')?.value;

      if (this.isEditing) {
        const alternative: UpdateAlternative = {
          id: this.alternativeIdEdited,
          text: text,
          isCorrect: isCorrect,
          questionId: questionId
        }

        this.alternativeService.update(alternative).subscribe({
          next: () => {
            this.errorMessage = null;
            this.resetForm();
            this.loadAlternatives();
          },
          error: err => {
            const serverError = err.error.errors?.[0];

            if (serverError?.includes('five')) {
              this.errorMessage = 'Erro ao atualizar alternativa: A questão selecionada já possui 5 alternativas.';
            } else if (serverError?.includes('correct')) {
              this.errorMessage = 'Erro ao atualizar alternativa: A questão selecionada já possui uma alternativa marcada como correta.';
            } else {
              this.errorMessage = 'Erro ao atualizar alternativa. Tente novamente.';
            }
          }
        });
      } else {
        const alternative: AddAlternative = {
          text: text,
          isCorrect: isCorrect,
          questionId: questionId
        }

        this.alternativeService.add(alternative).subscribe({
          next: () => {
            this.errorMessage = null;
            this.resetForm();
            this.loadAlternatives();
          },
          error: err => {
            const serverError = err.error.errors?.[0];

            if (serverError?.includes('five')) {
              this.errorMessage = 'Erro ao cadastrar alternativa: A questão selecionada já possui 5 alternativas.';
            } else if (serverError?.includes('correct')) {
              this.errorMessage = 'Erro ao cadastrar alternativa: A questão selecionada já possui uma alternativa marcada como correta.';
            } else {
              this.errorMessage = 'Erro ao cadastrar alternativa. Tente novamente.';
            }
          }
        });
      }
    }
  }

  resetForm(): void {
    this.formGroup.reset();
    this.isEditing = false;
  }

  loadAlternatives(): void {
    this.alternativeService.getAll().subscribe({
      next: alternatives => {
        this.errorMessage = null;
        this.alternatives = alternatives;
        this.loadQuestionStatements(alternatives);
      },
      error: () => this.errorMessage = 'Erro ao carregar alternativas. Tente novamente.'
    });
  }

  editAlternative(alternative: Alternative): void {
    this.alternativeIdEdited = alternative.id;
    this.isEditing = true;
    this.formGroup.patchValue(alternative);
  }

  deleteAlternative(id: number): void {
    if (confirm('Tem certeza que deseja excluir esta alternativa?')) {
      this.alternativeService.delete(id).subscribe({
        next: () => {
          this.errorMessage = null;
          this.loadAlternatives();
        },
        error: () => this.errorMessage = 'Erro ao deletar alternativa. Tente novamente.'
      });
    }
  }

  loadQuestions(): void {
    this.questionService.getAll().subscribe({
      next: questions => {
        this.errorMessage = null;
        this.questions = questions;
      },
      error: () => this.errorMessage = 'Erro ao carregar questões. Tente novamente.'
    });
  }

  loadQuestionStatements(alternatives: Alternative[]): void {
    alternatives.forEach(alternative => {
      const questionId = alternative.questionId;

      if (!this.questionStatements[questionId]) {
        this.questionService.getById(questionId).subscribe(question => {
          this.questionStatements[questionId] = question.statement;
        });
      }
    });
  }

  translateIsCorrect(isCorrect: boolean): string {
    return isCorrect ? 'Verdadeiro' : 'Falso';
  }
}
