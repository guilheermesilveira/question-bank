import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddAlternative, Alternative, SearchAlternative, UpdateAlternative } from '../models/alternative.model';
import { Observable } from 'rxjs';
import { Pagination } from '../models/pagination.model';

@Injectable({
  providedIn: 'root'
})
export class AlternativeService {

  private readonly api = '/backend/api/v1/alternative';

  constructor(private http: HttpClient) { }

  add(alternative: AddAlternative): Observable<Alternative> {
    return this.http.post<Alternative>(this.api, alternative);
  }

  update(alternative: UpdateAlternative): Observable<Alternative> {
    return this.http.put<Alternative>(`${this.api}/${alternative.id}`, alternative);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.api}/${id}`);
  }

  search(alternative: SearchAlternative): Observable<Pagination<Alternative>> {
    let params = new HttpParams();

    if (alternative.text) {
      params = params.set('Text', alternative.text);
    }

    if (alternative.isCorrect) {
      params = params.set('IsCorrect', alternative.isCorrect);
    }

    if (alternative.questionId) {
      params = params.set('QuestionId', alternative.questionId);
    }

    return this.http.get<Pagination<Alternative>>(`${this.api}/search`, { params });
  }

  getById(id: number): Observable<Alternative> {
    return this.http.get<Alternative>(`${this.api}/get-by-id/${id}`);
  }

  getAll(): Observable<Alternative[]> {
    return this.http.get<Alternative[]>(`${this.api}/get-all`);
  }
}
