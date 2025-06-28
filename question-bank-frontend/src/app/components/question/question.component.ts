import { Component } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Question } from 'src/app/models/question.model';

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent {

  formGroup!: FormGroup;
  questions: Question[] = [];
}
