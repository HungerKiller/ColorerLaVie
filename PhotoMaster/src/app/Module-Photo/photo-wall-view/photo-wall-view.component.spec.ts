import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PhotoWallViewComponent } from './photo-wall-view.component';

describe('PhotoWallViewComponent', () => {
  let component: PhotoWallViewComponent;
  let fixture: ComponentFixture<PhotoWallViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PhotoWallViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PhotoWallViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
