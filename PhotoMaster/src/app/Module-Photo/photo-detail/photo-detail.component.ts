import { Component, EventEmitter, OnInit, Input, Output } from '@angular/core';
import { LabelService } from '../../Services/label.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { PhotoService } from 'src/app/Services/photo.service';
import { Photo } from '../../Models/Photo';
import { Label } from '../../Models/Label';
import differenceInCalendarDays from 'date-fns/differenceInCalendarDays';

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
  labelIds: number[];

  listOfSelectedLabel: Label[];
  listOfAllLabel: Label[];

  title: string;
  isVisible: boolean;

  today = new Date();
  disabledDate = (current: Date): boolean => {
    // Can not select days before today and today
    return differenceInCalendarDays(current, this.today) > 0;
  };

  @Output() isNeedRefresh = new EventEmitter<boolean>();

  constructor(private labelService: LabelService, private photoService: PhotoService, private message: NzMessageService) { }

  ngOnInit(): void {
    this.getLabels();
  }

  close(): void {
    this.isVisible = false;
  }

  submit(): void {
    this.listOfSelectedLabel = [] as Label[];
    for (let lid of this.labelIds) {
      let label = this.listOfAllLabel.find(l => l.id == lid)
      this.listOfSelectedLabel.push(label);
    }
    console.log(this.listOfSelectedLabel);

    if (this.title == "Update") {
      this.photoService.putPhoto(this.photoId, new Photo(this.photoId, this.date.toISOString(), this.location, this.description, this.listOfSelectedLabel)) //todo labels
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
      this.photoService.postPhoto(new Photo(0, this.date.toISOString(), this.location, this.description, this.listOfSelectedLabel)) //todo labels
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

  getLabels(): void {
    this.labelService.getLabels().subscribe(labels => this.listOfAllLabel = labels);
  }
}
