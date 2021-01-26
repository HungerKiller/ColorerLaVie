import { Component, OnInit, ViewChild } from '@angular/core';
import { Label } from '../../Models/Label';
import { LabelService } from '../../Services/label.service';
import { LabelDetailComponent } from '../label-detail/label-detail.component';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-label-list',
  templateUrl: './label-list.component.html',
  styleUrls: ['./label-list.component.sass']
})
export class LabelListComponent implements OnInit {

  @ViewChild(LabelDetailComponent) labelDetailComponent: LabelDetailComponent;

  labels: Label[];

  constructor(private labelService: LabelService, private messageService: NzMessageService) { }

  ngOnInit(): void {
    this.getLabels();
  }

  getLabels(): void {
    this.labelService.getLabels().subscribe(labels => this.labels = labels);
  }

  editLabel(selectedLabel): void {
    this.labelDetailComponent.labelId = selectedLabel.id;
    this.labelDetailComponent.labelName = selectedLabel.name;
    if (this.labelDetailComponent.listOfSelectedColors.includes(selectedLabel.color)) {
      this.labelDetailComponent.colorMode = "Select";
      this.labelDetailComponent.selectedColor = selectedLabel.color;
    }
    else {
      this.labelDetailComponent.colorMode = "Custom";
      this.labelDetailComponent.pickedColor = selectedLabel.color;
    }
    this.labelDetailComponent.title = "Update";
    this.labelDetailComponent.isVisible = true;
  }

  createLabel(): void {
    this.labelDetailComponent.labelId = 0;
    this.labelDetailComponent.title = "Create";
    this.labelDetailComponent.isVisible = true;
  }

  deleteLabel(labelId: number): void {
    this.labelService.deleteLabel(labelId)
      .subscribe({
        next: data => {
          this.messageService.create("success", "Delete succeed!");
          this.getLabels();
        },
        error: error => {
          this.messageService.create("error", error.error);
        }
      });
  }

  refresh() {
    this.getLabels();
  }
}