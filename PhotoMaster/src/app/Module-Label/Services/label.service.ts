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

  postLabel(): Observable<Label> {
    return this.http.get<Label>(ApiRoute.LABEL.postLabel()); // todo add body
  }

  putLabel(labelId: number): Observable<Label> {
    return this.http.get<Label>(ApiRoute.LABEL.putLabel(labelId)); // todo add body
  }

  deleteLabel(labelId: number): Observable<Label> {
    return this.http.get<Label>(ApiRoute.LABEL.deleteLabel(labelId)); // todo return value
  }
}
