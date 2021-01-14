import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LabelComponent } from './label.component';
import { LabelDetailComponent } from './Components/label-detail/label-detail.component';
import { LabelListComponent } from './Components/label-list/label-list.component';

@NgModule({
  declarations: [LabelComponent, LabelDetailComponent, LabelListComponent],
  imports: [
    CommonModule
  ]
})
export class LabelModule { }
