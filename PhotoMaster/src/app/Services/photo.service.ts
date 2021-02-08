import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, observable, of } from 'rxjs'
import { ApiRoute } from 'src/app/api-routes';
import { Photo } from '../Models/Photo';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {

  get token(): any {
    return localStorage.getItem('token');
  }

  constructor(private http: HttpClient) { }

  getPhotos(): Observable<Photo[]> {
    return this.http.get<Photo[]>(ApiRoute.PHOTO.getPhotos(), { headers: new HttpHeaders().set('Authorization', `Bearer ${this.token}`) });
  }

  getPhoto(photoId: number): Observable<Photo> {
    return this.http.get<Photo>(ApiRoute.PHOTO.getPhoto(photoId), { headers: new HttpHeaders().set('Authorization', `Bearer ${this.token}`) });
  }

  postPhoto(photo: Photo): Observable<Photo> {
    return this.http.post<Photo>(ApiRoute.PHOTO.postPhoto(), photo, { headers: new HttpHeaders().set('Authorization', `Bearer ${this.token}`) });
  }

  putPhoto(photoId: number, photo: Photo): Observable<Photo> {
    return this.http.put<Photo>(ApiRoute.PHOTO.putPhoto(photoId), photo, { headers: new HttpHeaders().set('Authorization', `Bearer ${this.token}`) });
  }

  deletePhoto(photoId: number): Observable<Photo> {
    return this.http.delete<Photo>(ApiRoute.PHOTO.deletePhoto(photoId), { headers: new HttpHeaders().set('Authorization', `Bearer ${this.token}`) });
  }

  getPhotosByLabels(labelIds: number[]): Observable<Photo[]> {
    return this.http.get<Photo[]>(ApiRoute.PHOTO.getPhotosByLabels(labelIds), { headers: new HttpHeaders().set('Authorization', `Bearer ${this.token}`) });
  }

  uploadPhoto(photoId: number, formData: FormData): Observable<Photo> {
    return this.http.post<Photo>(ApiRoute.PHOTO.uploadPhoto(photoId), formData, { headers: new HttpHeaders().set('Authorization', `Bearer ${this.token}`) }); //todo reportProgress
  }

  uploadMultiPhotos(formData: FormData): Observable<Photo[]> {
    return this.http.post<Photo[]>(ApiRoute.PHOTO.uploadMultiPhotos(), formData, { headers: new HttpHeaders().set('Authorization', `Bearer ${this.token}`) }); //todo reportProgress
  }
}
