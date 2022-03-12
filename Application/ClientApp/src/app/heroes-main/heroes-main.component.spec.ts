import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeroesMainComponent } from './heroes-main.component';

describe('HeroesMainComponent', () => {
  let component: HeroesMainComponent;
  let fixture: ComponentFixture<HeroesMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HeroesMainComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HeroesMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
