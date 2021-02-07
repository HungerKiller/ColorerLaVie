import { Component, OnInit } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd/message';
import { ApiRoute } from 'src/app/api-routes';
import { Label } from 'src/app/Models/Label';
import { LabelService } from 'src/app/Services/label.service';
import { PhotoService } from 'src/app/Services/photo.service';

@Component({
  selector: 'app-photo-wall-view',
  templateUrl: './photo-wall-view.component.html',
  styleUrls: ['./photo-wall-view.component.sass']
})
export class PhotoWallViewComponent implements OnInit {

  urls = [] as string[];
  visible = false;
  listOfAllLabel: Label[];
  listOfSelectedLabelIds: number[];
  selectMode = "all";

  constructor(private labelService: LabelService, private photoService: PhotoService, private message: NzMessageService) { }

  ngOnInit() {
    this.getAllPhotoUrls();
    this.getLabels();
  }

  getAllPhotoUrls(): void {
    this.photoService.getPhotos().subscribe({
      next: data => {
        this.urls = [];
        for (let photo of data) {
          if (photo.path != null)
            this.urls.push(`${ApiRoute.APPSERVICEHOST}/${photo.path}`);
        }
      },
      error: error => {
        console.log(error.error);
      }
    });
  }

  getPhotoUrlsByLabels(): void {
    if (this.listOfSelectedLabelIds === undefined) {
      this.message.create("error", "Please select labels as filter");
      return;
    }
    this.photoService.getPhotosByLabels(this.listOfSelectedLabelIds)
      .subscribe({
        next: data => {
          this.message.create("success", "Filter photos!");
          this.urls = [];
          for (let photo of data) {
            if (photo.path != null)
              this.urls.push(`${ApiRoute.APPSERVICEHOST}/${photo.path}`);
          }
        },
        error: error => {
          this.message.create("error", error.error);
        }
      });
  }

  getLabels(): void {
    this.labelService.getLabels().subscribe(labels => this.listOfAllLabel = labels);
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

  open(): void {
    this.visible = true;
  }

  close(): void {
    this.visible = false;
    if (this.selectMode == "all")
      this.getAllPhotoUrls();
    else
      this.getPhotoUrlsByLabels();
  }
}