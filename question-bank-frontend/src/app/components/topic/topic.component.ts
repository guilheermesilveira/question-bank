import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AddTopic, Topic, UpdateTopic } from 'src/app/models/topic.model';
import { TopicService } from 'src/app/services/topic.service';

@Component({
  selector: 'app-topic',
  templateUrl: './topic.component.html',
  styleUrls: ['./topic.component.css']
})
export class TopicComponent implements OnInit {

  formGroup!: FormGroup;
  isEditing: boolean = false;
  topics: Topic[] = [];
  topicIdEdited!: number;
  errorMessage: string | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private topicService: TopicService
  ) { }

  ngOnInit(): void {
    this.initForm();
    this.loadTopics();
  }

  initForm(): void {
    this.formGroup = this.formBuilder.group({
      name: ['', [Validators.required]]
    });
  }

  onSubmitForm(): void {
    if (this.formGroup.valid) {
      const name = this.formGroup.get('name')?.value;

      if (this.isEditing) {
        const topic: UpdateTopic = {
          id: this.topicIdEdited,
          name: name
        }

        this.topicService.update(topic).subscribe({
          next: () => {
            this.errorMessage = null;
            this.resetForm();
            this.loadTopics();
          },
          error: () => this.errorMessage = 'Erro ao atualizar tópico. Tente novamente.'
        });
      } else {
        const topic: AddTopic = {
          name: name
        }

        this.topicService.add(topic).subscribe({
          next: () => {
            this.errorMessage = null;
            this.resetForm();
            this.loadTopics();
          },
          error: err => {
            const serverError = err.error.errors?.[0];

            if (serverError?.includes('Name')) {
              this.errorMessage = 'Erro ao cadastrar tópico: Já existe um tópico cadastrado com esse nome.';
            } else {
              this.errorMessage = 'Erro ao cadastrar tópico. Tente novamente.';
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

  loadTopics(): void {
    this.topicService.getAll().subscribe({
      next: topics => {
        this.errorMessage = null;
        this.topics = topics;
      },
      error: () => this.errorMessage = 'Erro ao carregar tópicos. Tente novamente.'
    });
  }

  editTopic(topic: Topic): void {
    this.topicIdEdited = topic.id;
    this.isEditing = true;
    this.formGroup.patchValue(topic);
  }

  deleteTopic(id: number): void {
    if (confirm('Tem certeza que deseja excluir este tópico?')) {
      this.topicService.delete(id).subscribe({
        next: () => {
          this.errorMessage = null;
          this.loadTopics();
        },
        error: () => this.errorMessage = 'Erro ao deletar tópico. Tente novamente.'
      });
    }
  }
}
