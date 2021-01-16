import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ContactComponent } from './Components/contact/contact.component';
import { LabelDetailComponent } from './Module-Label/Components/label-detail/label-detail.component';
import { LabelListComponent } from './Module-Label/Components/label-list/label-list.component';
import { LabelComponent } from './Module-Label/label.component';
import { DetailViewComponent } from './Module-Photo/Components/detail-view/detail-view.component';
import { GlobalViewComponent } from './Module-Photo/Components/global-view/global-view.component';
import { HomeViewComponent } from './Module-Photo/Components/home-view/home-view.component';
import { PhotoDetailComponent } from './Module-Photo/Components/photo-detail/photo-detail.component';
import { PhotoListComponent } from './Module-Photo/Components/photo-list/photo-list.component';
import { PhotoComponent } from './Module-Photo/photo.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeViewComponent },
  { path: 'global', component: GlobalViewComponent },
  { path: 'detail', component: DetailViewComponent },
  { path: 'contact', component: ContactComponent },
  {
    path: 'label',
    component: LabelComponent, // this is the component with the <router-outlet> in the template
    children: [
      {
        path: 'list', // child route path
        component: LabelListComponent, // child route component that the router renders
      },
      {
        path: 'detail',
        component: LabelDetailComponent, // another child route component that the router renders
      }
    ]
  },
  {
    path: 'photo',
    component: PhotoComponent,
    children: [
      {
        path: 'list',
        component: PhotoListComponent,
      },
      {
        path: 'detail',
        component: PhotoDetailComponent,
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
