import { Injectable } from '@angular/core';
import { StreamDto } from './StreamDto';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class StreamService {

  StreamUrl = '/api/Stream';
  constructor(private http: HttpClient) { }

  getStream(name: string) {
    return this.http.get<StreamDto>(`${this.StreamUrl}/${name}`);
  }
}
