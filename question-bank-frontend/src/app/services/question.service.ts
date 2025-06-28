import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddQuestion, Question, SearchQuestion, UpdateQuestion } from '../models/question.model';
import { Observable } from 'rxjs';
import { Pagination } from '../models/pagination.model';

@Injectable({
  providedIn: 'root'
})
export class QuestionService {

  private readonly api = '/backend/api/v1/question';

  constructor(private http: HttpClient) { }

  add(question: AddQuestion): Observable<Question> {
    return this.http.post<Question>(this.api, question);
  }

  update(question: UpdateQuestion): Observable<Question> {
    return this.http.put<Question>(`${this.api}/${question.id}`, question);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.api}/${id}`);
  }

  search(question: SearchQuestion): Observable<Pagination<Question>> {
    let params = new HttpParams();

    if (question.statement) {
      params = params.set('Statement', question.statement);
    }

    if (question.difficulty) {
      params = params.set('Difficulty', question.difficulty);
    }

    if (question.topicId) {
      params = params.set('TopicId', question.topicId);
    }

    return this.http.get<Pagination<Question>>(`${this.api}/search`, { params });
  }

  getById(id: number): Observable<Question> {
    return this.http.get<Question>(`${this.api}/get-by-id/${id}`);
  }

  getAll(): Observable<Question[]> {
    return this.http.get<Question[]>(`${this.api}/get-all`);
  }
}
