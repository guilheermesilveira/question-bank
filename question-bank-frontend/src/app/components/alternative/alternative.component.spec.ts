import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AlternativeComponent } from './alternative.component';

describe('AlternativeComponent', () => {
  let component: AlternativeComponent;
  let fixture: ComponentFixture<AlternativeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AlternativeComponent]
    });
    fixture = TestBed.createComponent(AlternativeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
