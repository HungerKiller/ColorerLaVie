import { Component, OnInit } from '@angular/core';
import { ApiRoute } from 'src/app/api-routes';
import { PhotoService } from 'src/app/Services/photo.service';

@Component({
  selector: 'app-home-view',
  templateUrl: './home-view.component.html',
  styleUrls: ['./home-view.component.sass']
})
export class HomeViewComponent implements OnInit {

  urls = [] as string[];

  constructor(private photoService: PhotoService) { }

  ngOnInit() {
    this.getPhotoUrl();
  }

  getPhotoUrl(): void {
    this.photoService.getPhotos().subscribe({
      next: data => {
        for (let photo of data) {
          if (photo.path != null)
            this.urls.push(`${ApiRoute.HOST}/${photo.path}`);
        }
      },
      error: error => {
        console.log(error.error);
      }
    });
  }

  onImageLoad(event) {
    if (event && event.target) {
      let gallery = document.querySelector('#gallery');
      var altura = parseInt(window.getComputedStyle(gallery).getPropertyValue('grid-auto-rows'));
      var gap = parseInt(window.getComputedStyle(gallery).getPropertyValue('grid-row-gap'));
      var item = event.target.parentElement.parentElement;
      item.style.gridRowEnd = "span " + Math.ceil((item.querySelector('.content').getBoundingClientRect().height + gap) / (altura + gap));
    }
  }

  onResize(event) {
    if (event && event.target) {
      let gallery = document.querySelector('#gallery');
      var altura = parseInt(window.getComputedStyle(gallery).getPropertyValue('grid-auto-rows'));
      var gap = parseInt(window.getComputedStyle(gallery).getPropertyValue('grid-row-gap'));
      gallery.querySelectorAll('.gallery-item').forEach(function (item) {
        var el = <HTMLInputElement>item;
        el.style.gridRowEnd = "span " + Math.ceil((item.querySelector('.content').getBoundingClientRect().height + gap) / (altura + gap));
      });
    }
  }
}