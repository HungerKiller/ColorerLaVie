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
  displayPhotos: Photo[];

  // Table function
  searchLocationValue: string;
  sortDate = ((a: Photo, b: Photo) => a.date.localeCompare(b.date));

  constructor(private photoService: PhotoService, private messageService: NzMessageService) { }

  ngOnInit(): void {
    this.refresh();
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
          this.refresh();
        },
        error: error => {
          this.messageService.create("error", error.error);
        }
      });
  }

  refresh() {
    this.getPhotos();
  }

  getPhotos(): void {
    this.photoService.getPhotos().subscribe(photos => { this.photos = photos; this.displayPhotos = photos; });
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

  // Filter
  reset(): void {
    this.searchLocationValue = '';
    this.searchLocation();
  }

  searchLocation(): void {
    this.displayPhotos = this.photos.filter((item: Photo) => item.location.indexOf(this.searchLocationValue) !== -1);
  }
}