import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CreateTest, Test } from 'src/app/models/test.model';
import { Topic } from 'src/app/models/topic.model';
import { TestService } from 'src/app/services/test.service';
import { TopicService } from 'src/app/services/topic.service';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements OnInit {

  formGroup!: FormGroup;
  errorMessage: string | null = null;
  currentUserId!: number;
  topics: Topic[] = [];
  currentTest!: Test;

  constructor(
    private formBuilder: FormBuilder,
    private testService: TestService,
    private topicService: TopicService
  ) { }

  ngOnInit(): void {
    this.getCurrentUserId();
    this.initForm();
    this.loadTopics();
  }

  initForm(): void {
    this.formGroup = this.formBuilder.group({
      userId: [this.currentUserId, [Validators.required]],
      title: ['', [Validators.required, Validators.maxLength(50)]],
      totalQuestions: [null, [Validators.required, Validators.min(1), Validators.max(20)]],
      difficulty: ['', [Validators.required]],
      topicIds: ['', [Validators.required]]
    });
  }

  onSubmitForm(): void {
    if (this.formGroup.valid) {
      const userId = this.formGroup.get('userId')?.value;
      const title = this.formGroup.get('title')?.value;
      const totalQuestions = this.formGroup.get('totalQuestions')?.value;
      const difficulty = this.formGroup.get('difficulty')?.value;
      const topicIds = this.formGroup.get('topicIds')?.value;
      const topicIdList: number[] = [];
      topicIdList.push(topicIds);

      const test: CreateTest = {
        userId: userId,
        title: title,
        totalQuestions: totalQuestions,
        difficulty: difficulty,
        topicIds: topicIdList
      }

      this.testService.create(test).subscribe({
        next: test => {
          this.errorMessage = null;
          this.currentTest = test;
          this.formGroup.reset();
        },
        error: err => {
          const serverError = err.error.errors?.[0];

          if (serverError?.includes('User')) {
            this.errorMessage = 'Erro ao criar simulado: Usuário não encontrado.';
          } else if (serverError?.includes('questions')) {
            this.errorMessage = 'Erro ao criar simulado: A quantidade de questões solicitada é superior ao que existe no banco atualmente.';
          } else {
            this.errorMessage = 'Erro ao criar simulado. Tente novamente.';
          }
        }
      });
    }
  }

  getCurrentUserId(): void {
    const token = localStorage.getItem('token');
    const payload = JSON.parse(atob(token!.split('.')[1]));
    this.currentUserId = payload.nameid;
  }

  loadTopics(): void {
    this.topicService.getAll().subscribe({
      next: topics => {
        this.errorMessage = null;
        this.topics = topics;
      },
      error: () => {
        this.errorMessage = 'Erro ao carregar tópicos. Tente novamente.';
      }
    });
  }
}
