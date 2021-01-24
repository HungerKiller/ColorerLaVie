import { Component, OnInit } from '@angular/core';
import { ApiRoute } from 'src/app/api-routes';
import { Photo } from 'src/app/Models/Photo';
import { PhotoService } from 'src/app/Services/photo.service';

@Component({
  selector: 'app-global-view',
  templateUrl: './global-view.component.html',
  styleUrls: ['./global-view.component.sass']
})
export class GlobalViewComponent implements OnInit {

  urls = [] as string[];
  zoomLevel = 3;
  zoomList = [24, 12, 8, 6, 4, 3, 2, 1];

  constructor(private photoService: PhotoService) { }

  ngOnInit(): void {
    this.getPhotoUrl();
  }

  getPhotoUrl(): void {
    this.photoService.getPhotos().subscribe({
      next: data => {
        for (let photo of data) {
          if(photo.path != null)
            this.urls.push(`${ApiRoute.HOST}/${photo.path}`);
        }
      },
      error: error => {
        console.log(error.error);
      }
    });
  }

  zoomIn() {
    if (this.zoomLevel >= 24)
      return;
    let index = this.zoomList.indexOf(this.zoomLevel);
    this.zoomLevel = this.zoomList[index - 1];
  }

  zoomOut() {
    if (this.zoomLevel <= 1)
      return;
    let index = this.zoomList.indexOf(this.zoomLevel);
    this.zoomLevel = this.zoomList[index + 1];
  }
}