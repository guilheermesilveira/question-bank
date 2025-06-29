import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AddQuestion, EDifficultyLevel, Question, UpdateQuestion } from 'src/app/models/question.model';
import { Topic } from 'src/app/models/topic.model';
import { QuestionService } from 'src/app/services/question.service';
import { TopicService } from 'src/app/services/topic.service';

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements OnInit {

  formGroup!: FormGroup;
  isEditing: boolean = false;
  questions: Question[] = [];
  questionIdEdited!: number;
  topics: Topic[] = [];
  topicNames: { [key: number]: string } = {};
  errorMessage: string | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private questionService: QuestionService,
    private topicService: TopicService
  ) { }

  ngOnInit(): void {
    this.initForm();
    this.loadQuestions();
    this.loadTopics();
  }

  initForm(): void {
    this.formGroup = this.formBuilder.group({
      statement: ['', [Validators.required]],
      difficulty: ['', [Validators.required]],
      topicId: ['', [Validators.required]]
    });
  }

  onSubmitForm(): void {
    if (this.formGroup.valid) {
      const statement = String(this.formGroup.get('statement')?.value);

      let difficulty: EDifficultyLevel;
      if (this.formGroup.get('difficulty')?.value === 'Easy') {
        difficulty = EDifficultyLevel.Easy;
      } else if (this.formGroup.get('difficulty')?.value === 'Medium') {
        difficulty = EDifficultyLevel.Medium;
      } else {
        difficulty = EDifficultyLevel.Hard;
      }

      const topicId = Number(this.formGroup.get('topicId')?.value);

      if (this.isEditing) {
        const question: UpdateQuestion = {
          id: this.questionIdEdited,
          statement: statement,
          difficulty: difficulty,
          topicId: topicId
        }

        this.questionService.update(question).subscribe({
          next: () => {
            this.errorMessage = null;
            this.resetForm();
            this.loadQuestions();
          },
          error: () => this.errorMessage = 'Erro ao atualizar questão. Tente novamente.'
        });
      } else {
        const question: AddQuestion = {
          statement: statement,
          difficulty: difficulty,
          topicId: topicId
        }

        this.questionService.add(question).subscribe({
          next: () => {
            this.errorMessage = null;
            this.resetForm();
            this.loadQuestions();
          },
          error: () => this.errorMessage = 'Erro ao cadastrar questão. Tente novamente.'
        });
      }
    }
  }

  resetForm(): void {
    this.formGroup.reset();
    this.isEditing = false;
  }

  loadQuestions(): void {
    this.questionService.getAll().subscribe({
      next: questions => {
        this.errorMessage = null;
        this.questions = questions;
        this.loadTopicNames(questions);
      },
      error: () => this.errorMessage = 'Erro ao carregar questões. Tente novamente.'
    });
  }

  editQuestion(question: Question): void {
    this.questionIdEdited = question.id;
    this.isEditing = true;
    this.formGroup.patchValue(question);
  }

  deleteQuestion(id: number): void {
    if (confirm('Tem certeza que deseja excluir esta questão?')) {
      this.questionService.delete(id).subscribe({
        next: () => {
          this.errorMessage = null;
          this.loadQuestions();
        },
        error: () => this.errorMessage = 'Erro ao deletar questão. Tente novamente.'
      });
    }
  }

  loadTopics(): void {
    this.topicService.getAll().subscribe({
      next: topics => {
        this.errorMessage = null;
        this.topics = topics;
      },
      error: () => this.errorMessage = 'Erro ao carregar tópicos. Tente novamente.'
    });
  }

  loadTopicNames(questions: Question[]): void {
    questions.forEach(question => {
      const topicId = question.topicId;

      if (!this.topicNames[topicId]) {
        this.topicService.getById(topicId).subscribe(topic => {
          this.topicNames[topicId] = topic.name;
        });
      }
    });
  }

  translateDifficulty(difficulty: EDifficultyLevel): string {
    if (difficulty === 'Easy') {
      return 'Fácil';
    } else if (difficulty === 'Medium') {
      return 'Médio';
    } else {
      return 'Difícil';
    }
  }
}
