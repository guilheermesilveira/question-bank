import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Pagination } from 'src/app/models/pagination.model';
import { AddUser, SearchUser, UpdateUser, User } from 'src/app/models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  add(user: AddUser): Observable<User> {
    return this.http.post<User>('/backend/api/v1/user', { user });
  }

  update(user: UpdateUser): Observable<User> {
    return this.http.put<User>(`/backend/api/v1/user/${user.id}`, { user });
  }

  search(user: SearchUser): Observable<Pagination<User>> {
    let params = new HttpParams();

    if (user.name) {
      params = params.set('name', user.name);
    }

    if (user.email) {
      params = params.set('email', user.email);
    }

    return this.http.get<Pagination<User>>('/backend/api/v1/user/search', { params });
  }

  getById(id: number): Observable<User> {
    return this.http.get<User>(`/backend/api/v1/user/get-by-id/${id}`);
  }

  getAll(): Observable<User[]> {
    return this.http.get<User[]>('/backend/api/v1/user/get-all');
  }
}
