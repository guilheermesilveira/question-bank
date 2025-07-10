import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddOption, Option, SearchOption, UpdateOption } from '../models/option.model';
import { Observable } from 'rxjs';
import { Pagination } from '../models/pagination.model';

@Injectable({
  providedIn: 'root'
})
export class OptionService {

  private readonly api = '/backend/api/v1/option';

  constructor(private http: HttpClient) { }

  add(option: AddOption): Observable<Option> {
    return this.http.post<Option>(this.api, option);
  }

  update(option: UpdateOption): Observable<Option> {
    return this.http.put<Option>(`${this.api}/${option.id}`, option);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.api}/${id}`);
  }

  search(option: SearchOption): Observable<Pagination<Option>> {
    let params = new HttpParams();

    if (option.text) {
      params = params.set('Text', option.text);
    }

    if (option.isCorrect) {
      params = params.set('IsCorrect', option.isCorrect);
    }

    if (option.questionId) {
      params = params.set('QuestionId', option.questionId);
    }

    return this.http.get<Pagination<Option>>(`${this.api}/search`, { params });
  }

  getById(id: number): Observable<Option> {
    return this.http.get<Option>(`${this.api}/${id}`);
  }

  getAll(): Observable<Option[]> {
    return this.http.get<Option[]>(`${this.api}`);
  }
}
