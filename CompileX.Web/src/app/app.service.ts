import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environment/environment';

export interface CodeRunRequest {
  code: string;
  language: string;
}

export interface CodeRunResponse {
  output: string;
  error: string;
}

@Injectable({
  providedIn: 'root'
})
export class CodeRunnerService {

  private baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  runCode(data: CodeRunRequest): Observable<CodeRunResponse> {
    return this.http.post<CodeRunResponse>(
      `${this.baseUrl}/api/code/run`,
      data
    );
  }
}
