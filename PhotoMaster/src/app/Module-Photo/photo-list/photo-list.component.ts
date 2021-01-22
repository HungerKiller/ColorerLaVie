import { Component, OnInit, ViewChild } from '@angular/core';
import { Photo } from '../../Models/Photo';
import { PhotoService } from '../../Services/photo.service';
import { PhotoDetailComponent } from '../photo-detail/photo-detail.component';
import { NzMessageService } from 'ng-zorro-antd/message';

import { ApiRoute } from 'src/app/api-routes';
import { NzUploadFile } from 'ng-zorro-antd/upload';
import { NzUploadChangeParam } from 'ng-zorro-antd/upload';

@Component({
  selector: 'app-photo-list',
  templateUrl: './photo-list.component.html',
  styleUrls: ['./photo-list.component.sass']
})

export class PhotoListComponent implements OnInit {

  @ViewChild(PhotoDetailComponent) photoDetailComponent: PhotoDetailComponent;

  url: string;
  photos: Photo[];

  constructor(private photoService: PhotoService, private messageService: NzMessageService) { }

  ngOnInit(): void {
    this.getPhotos();
  }

  getPhotos(): void {
    this.photoService.getPhotos().subscribe(photos => this.photos = photos);
  }

  editPhoto(selectedPhoto): void {
    this.photoDetailComponent.photoId = selectedPhoto.id;
    this.photoDetailComponent.date = new Date(selectedPhoto.date);
    this.photoDetailComponent.location = selectedPhoto.location;
    this.photoDetailComponent.description = selectedPhoto.description;
    let ids = <number[]>[];
    selectedPhoto.labels.forEach(function (value) {
      ids.push(value.id);
    });
    this.photoDetailComponent.labelIds = ids;

    this.photoDetailComponent.title = "Update";
    this.photoDetailComponent.isVisible = true;
  }

  createPhoto(): void {
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

  refresh() {
    this.getPhotos();
  }

  setUrl(photoId: number): void {
    this.url = ApiRoute.PHOTO.uploadPhoto(photoId);
  }

  handleChange(info: NzUploadChangeParam): void {
    // if (info.file.status !== 'uploading') {
    //   console.log(info.file, info.fileList);
    // }
    if (info.file.status === 'done') {
      this.messageService.success(`${info.file.name} file uploaded successfully`);
    } else if (info.file.status === 'error') {
      this.messageService.error(`${info.file.name} file upload failed.`);
    }
  }
}