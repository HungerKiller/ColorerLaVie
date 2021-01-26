import { Component, EventEmitter, OnInit, Input, Output } from '@angular/core';
import { LabelService } from '../../Services/label.service';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-label-detail',
  templateUrl: './label-detail.component.html',
  styleUrls: ['./label-detail.component.sass']
})
export class LabelDetailComponent implements OnInit {

  labelId: number;
  labelName: string;
  labelColor: string;
  title: string;
  isVisible: boolean;
  // Color
  colorMode = "Select";
  selectedColor = "red";
  listOfSelectedColors = ["red", "cyan", "blue", "purple", "yellow", "lime", "magenta", "orange", "green", "volcano", "gold", "geekblue"];
  pickedColor = "#3498DB";

  @Output() isNeedRefresh = new EventEmitter<boolean>();

  constructor(private labelService: LabelService, private message: NzMessageService) { }

  ngOnInit(): void {
  }

  close(): void {
    this.isVisible = false;
  }

  submit(): void {
    if (this.title == "Update") {
      let color = (this.colorMode == "Select") ? this.selectedColor : this.pickedColor;
      this.labelService.putLabel(this.labelId, { id: this.labelId, name: this.labelName, color: color })
        .subscribe({
          next: data => {
            this.labelName = data.name;
            this.message.create("success", "Update succeed!");
            this.close();
            this.isNeedRefresh.emit();
          },
          error: error => {
            this.message.create("error", error.error);
          }
        });
    }
    else if (this.title == "Create") {
      let color = (this.colorMode == "Select") ? this.selectedColor : this.pickedColor;
      this.labelService.postLabel({ id: 0, name: this.labelName, color: color })
        .subscribe({
          next: data => {
            this.labelName = data.name;
            this.message.create("success", "Create succeed!");
            this.close();
            this.isNeedRefresh.emit();
          },
          error: error => {
            this.message.create("error", error.error);
          }
        });
    }
  }
}
