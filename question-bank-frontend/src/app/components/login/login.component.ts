import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from 'src/app/services/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  formGroup!: FormGroup;
  credentialsValid: boolean = true;

  constructor(
    private loginService: LoginService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {
    this.initForm();
  }

  initForm(): void {
    this.formGroup = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  onSubmitForm(): void {
    if (this.formGroup.valid) {
      const email = this.formGroup.get('email')?.value;
      const password = this.formGroup.get('password')?.value;

      this.loginService.login(email, password).subscribe({
        next: () => {
          this.credentialsValid = true;
          this.router.navigate(['/dashboard']);
        },
        error: () => this.credentialsValid = false
      });
    }
  }
}
