import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WeeklyViewComponent } from './weekly-view.component';

describe('WeeklyViewComponent', () => {
  let component: WeeklyViewComponent;
  let fixture: ComponentFixture<WeeklyViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WeeklyViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WeeklyViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
