import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FinishTest, Test } from 'src/app/models/test.model';
import { TestService } from 'src/app/services/test.service';

@Component({
  selector: 'app-test-answer',
  templateUrl: './test-answer.component.html',
  styleUrls: ['./test-answer.component.css']
})
export class TestAnswerComponent implements OnInit {

  errorMessage: string | null = null;
  currentTest!: Test;
  formGroup!: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private testService: TestService,
    private formBuilder: FormBuilder,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadTest();
  }

  loadTest(): void {
    const testId = Number(this.route.snapshot.paramMap.get('id'));
    this.testService.getById(testId).subscribe({
      next: test => {
        this.currentTest = test;
        this.initForm();
      },
      error: () => this.onBack()
    });
  }

  initForm(): void {
    this.formGroup = this.formBuilder.group({
      questions: this.formBuilder.array([])
    });

    this.currentTest.testQuestions.forEach(() => {
      this.questionForms.push(
        this.formBuilder.group({
          selectedOption: [null, [Validators.required]]
        })
      );
    });
  }

  get questionForms() {
    return this.formGroup.get('questions') as FormArray;
  }

  onSubmitForm(): void {
    if (this.formGroup.valid) {
      const answers = this.formGroup.value.questions.map((answer: any, index: number) => ({
        questionId: this.currentTest.testQuestions[index].question.id,
        selectedOptionId: answer.selectedOption
      }));

      const test: FinishTest = {
        testId: this.currentTest.id,
        answers: answers
      }

      this.testService.finish(test).subscribe({
        next: () => this.errorMessage = null,
        error: () => this.errorMessage = 'Erro ao finalizar simulado. Tente novamente.'
      });
    }
  }

  getCorrectOptionText(questionIndex: number): string {
    const options = this.currentTest.testQuestions[questionIndex].question.options;
    const correct = options.find(opt => opt.isCorrect);
    return correct?.text ?? 'Alternativa correta não informada';
  }

  onBack(): void {
    this.router.navigate(['/tests']);
  }
}
