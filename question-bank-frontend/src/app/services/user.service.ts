import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Pagination } from 'src/app/models/pagination.model';
import { AddUser, SearchUser, UpdateUser, User } from 'src/app/models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private readonly api = '/backend/api/v1/user';

  constructor(private http: HttpClient) { }

  add(user: AddUser): Observable<User> {
    return this.http.post<User>(this.api, user);
  }

  update(user: UpdateUser): Observable<User> {
    return this.http.put<User>(`${this.api}/${user.id}`, user);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.api}/${id}`);
  }

  search(user: SearchUser): Observable<Pagination<User>> {
    let params = new HttpParams();

    if (user.name) {
      params = params.set('Name', user.name);
    }

    if (user.email) {
      params = params.set('Email', user.email);
    }

    return this.http.get<Pagination<User>>(`${this.api}/search`, { params });
  }

  getById(id: number): Observable<User> {
    return this.http.get<User>(`${this.api}/get-by-id/${id}`);
  }

  getAll(): Observable<User[]> {
    return this.http.get<User[]>(`${this.api}/get-all`);
  }
}
