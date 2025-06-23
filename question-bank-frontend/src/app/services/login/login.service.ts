import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { TokenModel } from 'src/app/models/token.model';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) { }

  login(email: string, password: string): Observable<TokenModel> {
    return this.http.post<TokenModel>('/backend/api/v1/auth/login', { email, password }).pipe(
      tap(response => {
        localStorage.setItem('token', response.token)
      })
    );
  }
}
