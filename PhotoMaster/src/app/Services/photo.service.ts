import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, observable, of } from 'rxjs'
import { ApiRoute } from 'src/app/api-routes';
import { Photo } from '../Models/Photo';

@Injectable({
  providedIn: 'root'
})
export class PhotoService {

  constructor(private http: HttpClient) { }

  getPhotos(): Observable<Photo[]> {
    return this.http.get<Photo[]>(ApiRoute.PHOTO.getPhotos());
  }

  getPhoto(photoId: number): Observable<Photo> {
    return this.http.get<Photo>(ApiRoute.PHOTO.getPhoto(photoId));
  }

  postPhoto(photo: Photo): Observable<Photo> {
    return this.http.post<Photo>(ApiRoute.PHOTO.postPhoto(), photo);
  }

  putPhoto(photoId: number, photo: Photo): Observable<Photo> {
    return this.http.put<Photo>(ApiRoute.PHOTO.putPhoto(photoId), photo);
  }

  deletePhoto(photoId: number): Observable<Photo> {
    return this.http.delete<Photo>(ApiRoute.PHOTO.deletePhoto(photoId));
  }

  getPhotosByLabels(labelIds: number[]): Observable<Photo[]> {
    return this.http.get<Photo[]>(ApiRoute.PHOTO.getPhotosByLabels(labelIds));
  }

  uploadPhoto(photoId: number, formData: FormData): Observable<Photo> {
    return this.http.post<Photo>(ApiRoute.PHOTO.uploadPhoto(photoId), formData); //todo reportProgress
  }

  uploadMultiPhotos(formData: FormData): Observable<Photo[]> {
    return this.http.post<Photo[]>(ApiRoute.PHOTO.uploadMultiPhotos(), formData); //todo reportProgress
  }
}
