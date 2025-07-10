import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AddOption, Option, UpdateOption } from 'src/app/models/option.model';
import { Question } from 'src/app/models/question.model';
import { OptionService } from 'src/app/services/option.service';
import { QuestionService } from 'src/app/services/question.service';

@Component({
  selector: 'app-option',
  templateUrl: './option.component.html',
  styleUrls: ['./option.component.css']
})
export class OptionComponent implements OnInit {

  formGroup!: FormGroup;
  isEditing: boolean = false;
  options: Option[] = [];
  optionIdEdited!: number;
  questions: Question[] = [];
  questionStatements: { [key: number]: string } = {};
  errorMessage: string | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private optionService: OptionService,
    private questionService: QuestionService
  ) { }

  ngOnInit(): void {
    this.initForm();
    this.loadOptions();
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
        const option: UpdateOption = {
          id: this.optionIdEdited,
          text: text,
          isCorrect: isCorrect,
          questionId: questionId
        }

        this.optionService.update(option).subscribe({
          next: () => {
            this.errorMessage = null;
            this.resetForm();
            this.loadOptions();
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
        const option: AddOption = {
          text: text,
          isCorrect: isCorrect,
          questionId: questionId
        }

        this.optionService.add(option).subscribe({
          next: () => {
            this.errorMessage = null;
            this.resetForm();
            this.loadOptions();
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

  loadOptions(): void {
    this.optionService.getAll().subscribe({
      next: options => {
        this.errorMessage = null;
        this.options = options;
        this.loadQuestionStatements(options);
      },
      error: () => this.errorMessage = 'Erro ao carregar alternativas. Tente novamente.'
    });
  }

  editOption(option: Option): void {
    this.optionIdEdited = option.id;
    this.isEditing = true;
    this.formGroup.patchValue(option);
  }

  deleteOption(id: number): void {
    if (confirm('Tem certeza que deseja excluir esta alternativa?')) {
      this.optionService.delete(id).subscribe({
        next: () => {
          this.errorMessage = null;
          this.loadOptions();
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

  loadQuestionStatements(options: Option[]): void {
    options.forEach(option => {
      const questionId = option.questionId;

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
