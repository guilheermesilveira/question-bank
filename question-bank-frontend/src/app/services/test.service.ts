import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TestService {

  private readonly api = '/backend/api/v1/test';

  constructor(private http: HttpClient) { }
}
