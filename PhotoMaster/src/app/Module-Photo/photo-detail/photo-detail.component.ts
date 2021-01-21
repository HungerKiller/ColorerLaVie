import { Component, EventEmitter, OnInit, Input, Output } from '@angular/core';
import { LabelService } from '../../Services/label.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { PhotoService } from 'src/app/Services/photo.service';
import differenceInCalendarDays from 'date-fns/differenceInCalendarDays';
import { Photo } from '../../Models/Photo';
import { Label } from '../../Models/Label';

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
  labels: Label[];
  title: string;
  isVisible: boolean;
  today = new Date();

  @Output() isNeedRefresh = new EventEmitter<boolean>();

  constructor(private labelService: LabelService, private photoService: PhotoService, private message: NzMessageService) { }

  ngOnInit(): void {
  }

  close(): void {
    this.isVisible = false;
  }

  submit(): void {
    if (this.title == "Update") {
      this.photoService.putPhoto(this.photoId, new Photo(this.photoId, this.date, this.location, this.description, this.labels)) //todo labels
        .subscribe({
          next: data => {
            this.message.create("success", "Update succeed!");
            this.close();
            this.isNeedRefresh.emit();
          },
          error: error => {
            this.message.create("error", error.error);
          }
        });
      // todo upload photo (path)
    }
    else if (this.title == "Create") {
      this.photoService.postPhoto(new Photo(0, this.date, this.location, this.description, this.labels)) //todo labels
        .subscribe({
          next: data => {
            this.message.create("success", "Create succeed!");
            this.close();
            this.isNeedRefresh.emit();
          },
          error: error => {
            this.message.create("error", error.error);
          }
        });
      // todo upload photo (path)
    }
  }

  disabledDate = (current: Date): boolean => {
    // Can not select days before today and today
    return differenceInCalendarDays(current, this.today) > 0;
  };
}
