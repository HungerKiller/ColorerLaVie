import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-photo-detail',
  templateUrl: './photo-detail.component.html',
  styleUrls: ['./photo-detail.component.sass']
})
export class PhotoDetailComponent implements OnInit {

  photoId: number;
    date: Date;
    path: string;
    location: string;
    description: string;
    // Labels: Label[];

    title : string;
  isVisible : boolean;

  constructor() { }

  ngOnInit(): void {
  }

}
