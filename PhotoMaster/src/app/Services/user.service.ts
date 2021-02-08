import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiRoute } from '../api-routes';
import { User } from '../Models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  getToken(user: User): Observable<User> {
    return this.http.post<User>(ApiRoute.USER.getToken(), user);
  }
}
