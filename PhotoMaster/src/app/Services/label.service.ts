import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, observable, of } from 'rxjs'
import { ApiRoute } from 'src/app/api-routes';
import { Label } from '../Models/Label';

@Injectable({
  providedIn: 'root'
})
export class LabelService {

  get token(): any {
    return localStorage.getItem('token');
  }

  constructor(private http: HttpClient) { }

  getLabels(): Observable<Label[]> {
    return this.http.get<Label[]>(ApiRoute.LABEL.getLabels(), {headers: new HttpHeaders().set('Authorization', `Bearer ${this.token}`)});
  }

  getLabel(labelId: number): Observable<Label> {
    return this.http.get<Label>(ApiRoute.LABEL.getLabel(labelId), {headers: new HttpHeaders().set('Authorization', `Bearer ${this.token}`)});
  }

  postLabel(label: Label): Observable<Label> {
    return this.http.post<Label>(ApiRoute.LABEL.postLabel(), label, {headers: new HttpHeaders().set('Authorization', `Bearer ${this.token}`)});
  }

  putLabel(labelId: number, label: Label): Observable<Label> {
    return this.http.put<Label>(ApiRoute.LABEL.putLabel(labelId), label, {headers: new HttpHeaders().set('Authorization', `Bearer ${this.token}`)});
  }

  deleteLabel(labelId: number): Observable<Label> {
    return this.http.delete<Label>(ApiRoute.LABEL.deleteLabel(labelId), {headers: new HttpHeaders().set('Authorization', `Bearer ${this.token}`)});
  }
}
