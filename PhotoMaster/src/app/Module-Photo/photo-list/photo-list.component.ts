import { Component, OnInit, ViewChild } from '@angular/core';
import { Photo } from '../../Models/Photo';
import { PhotoService } from '../../Services/photo.service';
import { PhotoDetailComponent } from '../photo-detail/photo-detail.component';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-photo-list',
  templateUrl: './photo-list.component.html',
  styleUrls: ['./photo-list.component.sass']
})

export class PhotoListComponent implements OnInit {

  @ViewChild(PhotoDetailComponent) photoDetailComponent : PhotoDetailComponent;

  photos: Photo[];

  constructor(private photoService: PhotoService, private messageService: NzMessageService) { }

  ngOnInit(): void {
    this.getPhotos();
  }

  getPhotos(): void {
    this.photoService.getPhotos().subscribe(photos => this.photos = photos);
  }

  editPhoto(selectedPhoto): void{
    this.photoDetailComponent.photoId = selectedPhoto.id;
    this.photoDetailComponent.date = new Date(selectedPhoto.date);
    this.photoDetailComponent.location = selectedPhoto.location;
    this.photoDetailComponent.description = selectedPhoto.description;
    this.photoDetailComponent.title = "Update";
    this.photoDetailComponent.isVisible = true;
  }

  createPhoto(): void{
    this.photoDetailComponent.photoId = 0;
    this.photoDetailComponent.title = "Create";
    this.photoDetailComponent.isVisible = true;
  }

  deletePhoto(photoId: number): void {
    this.photoService.deletePhoto(photoId)
    .subscribe({
      next: data => {
          this.messageService.create("success", "Delete succeed!");
          this.getPhotos();
      },
      error: error => {
          this.messageService.create("error", error.error);
      }
  });
  }

  refresh(){
    this.getPhotos();
  }
}