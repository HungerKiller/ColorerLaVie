import { Component, OnInit } from '@angular/core';
import { Label } from '../../Models/Label';
import { LabelService } from '../../Services/label.service';

@Component({
  selector: 'app-label-list',
  templateUrl: './label-list.component.html',
  styleUrls: ['./label-list.component.sass']
})
export class LabelListComponent implements OnInit {

  labels: Label[];

  constructor(private labelService: LabelService) { }

  ngOnInit(): void {
    this.getLabels();
  }

  getLabels(): void {
    this.labelService.getLabels().subscribe(labels => this.labels = labels);
  }
}
