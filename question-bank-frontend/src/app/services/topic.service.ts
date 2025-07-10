import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddTopic, SearchTopic, Topic, UpdateTopic } from '../models/topic.model';
import { Observable } from 'rxjs';
import { Pagination } from '../models/pagination.model';

@Injectable({
  providedIn: 'root'
})
export class TopicService {

  private readonly api = '/backend/api/v1/topic';

  constructor(private http: HttpClient) { }

  add(topic: AddTopic): Observable<Topic> {
    return this.http.post<Topic>(this.api, topic);
  }

  update(topic: UpdateTopic): Observable<Topic> {
    return this.http.put<Topic>(`${this.api}/${topic.id}`, topic);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.api}/${id}`);
  }

  search(topic: SearchTopic): Observable<Pagination<Topic>> {
    let params = new HttpParams();

    if (topic.name) {
      params = params.set('Name', topic.name);
    }

    return this.http.get<Pagination<Topic>>(`${this.api}/search`, { params });
  }

  getById(id: number): Observable<Topic> {
    return this.http.get<Topic>(`${this.api}/${id}`);
  }

  getAll(): Observable<Topic[]> {
    return this.http.get<Topic[]>(`${this.api}`);
  }
}
