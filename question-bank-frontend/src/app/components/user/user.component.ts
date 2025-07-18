import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AddUser, UpdateUser, User } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {

  formGroup!: FormGroup;
  isEditing: boolean = false;
  users: User[] = [];
  userIdEdited!: number;
  errorMessage: string | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    this.initForm();
    this.loadUsers();
  }

  initForm(): void {
    this.formGroup = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.pattern("^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$")]]
    });
  }

  onSubmitForm(): void {
    if (this.formGroup.valid) {
      const name = this.formGroup.get('name')?.value;
      const email = this.formGroup.get('email')?.value;
      const password = this.formGroup.get('password')?.value;

      if (this.isEditing) {
        const user: UpdateUser = {
          id: this.userIdEdited,
          name: name,
          email: email,
          password: password
        }

        this.userService.update(user).subscribe({
          next: () => {
            this.errorMessage = null;
            this.resetForm();
            this.loadUsers();
          },
          error: err => {
            const serverError = err.error.errors?.[0];

            if (serverError?.includes('Email')) {
              this.errorMessage = 'Erro ao atualizar usuário: Já existe um usuário utilizando o e-mail informado.';
            } else {
              this.errorMessage = 'Erro ao atualizar usuário. Tente novamente.';
            }
          }
        });
      } else {
        const user: AddUser = {
          name: name,
          email: email,
          password: password
        }

        this.userService.add(user).subscribe({
          next: () => {
            this.errorMessage = null;
            this.resetForm();
            this.loadUsers();
          },
          error: err => {
            const serverError = err.error.errors?.[0];

            if (serverError?.includes('Email')) {
              this.errorMessage = 'Erro ao cadastrar usuário: Já existe um usuário utilizando o e-mail informado.';
            } else {
              this.errorMessage = 'Erro ao cadastrar usuário. Tente novamente.';
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

  loadUsers(): void {
    this.userService.getAll().subscribe({
      next: users => {
        this.errorMessage = null;
        this.users = users;
      },
      error: () => this.errorMessage = 'Erro ao carregar usuários. Tente novamente.'
    });
  }

  editUser(user: User): void {
    this.userIdEdited = user.id;
    this.isEditing = true;
    this.formGroup.patchValue(user);
  }

  deleteUser(id: number): void {
    if (confirm('Tem certeza que deseja excluir este usuário?')) {
      this.userService.delete(id).subscribe({
        next: () => {
          this.errorMessage = null;
          this.loadUsers();
        },
        error: () => this.errorMessage = 'Erro ao deletar usuário. Tente novamente.'
      });
    }
  }
}
