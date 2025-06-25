import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from 'src/app/services/login/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  formGroup!: FormGroup;
  credentialsValid: boolean = true;

  constructor(
    private service: LoginService,
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

  onSubmit(): void {
    if (this.formGroup.valid) {
      const email = String(this.formGroup.get('email')?.value);
      const password = String(this.formGroup.get('password')?.value);
      this.service.login(email, password).subscribe({
        next: () => {
          this.credentialsValid = true;
          this.router.navigate(['/dashboard']);
        },
        error: () => this.credentialsValid = false
      });
    }
  }
}
