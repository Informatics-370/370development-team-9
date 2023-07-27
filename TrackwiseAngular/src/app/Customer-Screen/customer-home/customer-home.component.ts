// customer-home.component.ts
import { Component, OnInit, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-customer-home',
  templateUrl: './customer-home.component.html',
  styleUrls: ['./customer-home.component.scss']
})
export class CustomerHomeComponent implements OnInit, OnDestroy {
  images: { url: string }[] = [
    { url: '/assets/CustHome/c1.jpg' },
    { url: '/assets/CustHome/c2.jpg' },
    { url: '/assets/CustHome/c3.jpg' },
    { url: '/assets/CustHome/c4.jpg' },
    { url: '/assets/CustHome/c5.jpg' },
    { url: '/assets/CustHome/c6.jpg' },
    // Add more images as needed
  ];

  currentSlideIndex: number = 0;
  interval: any;

  ngOnInit(): void {
    this.startSlideshow();
  }

  ngOnDestroy(): void {
    this.stopSlideshow();
  }

  startSlideshow(): void {
    this.interval = setInterval(() => {
      this.slideToNext();
    }, 4000); // Change this value to adjust slide duration (now set to 4 seconds per image)
  }

  stopSlideshow(): void {
    clearInterval(this.interval);
  }

  slideToNext(): void {
    this.currentSlideIndex = (this.currentSlideIndex + 1) % this.images.length;
  }
}
