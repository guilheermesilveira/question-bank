import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateTest, FinishTest, Test } from '../models/test.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TestService {

  private readonly api = '/backend/api/v1/test';

  constructor(private http: HttpClient) { }

  create(test: CreateTest): Observable<Test> {
    return this.http.post<Test>(this.api, test);
  }

  finish(test: FinishTest): Observable<Test> {
    return this.http.post<Test>(`${this.api}/finish`, test);
  }

  getById(id: number): Observable<Test> {
    return this.http.get<Test>(`${this.api}/${id}`);
  }
}
