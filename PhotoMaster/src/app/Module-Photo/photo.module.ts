import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DetailViewComponent } from '../Module-Photo/Components/detail-view/detail-view.component';
import { GlobalViewComponent } from '../Module-Photo/Components/global-view/global-view.component';
import { HomeViewComponent } from '../Module-Photo/Components/home-view/home-view.component';
import { PhotoComponent } from './photo.component';
import { PhotoListComponent } from './Components/photo-list/photo-list.component';
import { PhotoDetailComponent } from './Components/photo-detail/photo-detail.component';



@NgModule({
  declarations: [DetailViewComponent, GlobalViewComponent, HomeViewComponent, PhotoComponent, PhotoListComponent, PhotoDetailComponent],
  imports: [
    CommonModule
  ]
})
export class PhotoModule { }
