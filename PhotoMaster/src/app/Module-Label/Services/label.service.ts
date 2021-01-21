import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, observable, of } from 'rxjs'
import { ApiRoute } from 'src/app/api-routes';
import { Label } from '../Models/Label';

@Injectable({
  providedIn: 'root'
})
export class LabelService {

  constructor(private http: HttpClient) { }

  getLabels(): Observable<Label[]> {
    return this.http.get<Label[]>(ApiRoute.LABEL.getLabels());
  }

  getLabel(labelId: number): Observable<Label> {
    return this.http.get<Label>(ApiRoute.LABEL.getLabel(labelId));
  }

  postLabel(label: Label): Observable<Label> {
    return this.http.post<Label>(ApiRoute.LABEL.postLabel(), label);
  }

  putLabel(labelId: number, label: Label): Observable<Label> {
    return this.http.put<Label>(ApiRoute.LABEL.putLabel(labelId), label);
  }

  deleteLabel(labelId: number): Observable<Label> {
    return this.http.delete<Label>(ApiRoute.LABEL.deleteLabel(labelId));
  }
}
