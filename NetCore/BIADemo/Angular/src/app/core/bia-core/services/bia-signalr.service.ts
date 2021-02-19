import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from 'src/environments/environment';

/**
 * Service managing the SignalR connection.
 * You can add specific events management by doing the following:
 * - Add a SignalRService parameter to the constructor of your component (for dependency injection)
 * - Use this injected instance of SignalRService to add new events:
 *     this.signalRService.getHubConnection().on('MyEventName', () => {
 *       this.store.select(getMyEventHandler).pipe(first()).subscribe(
 *         (myEventData) => {
 *             // Do what you need here...
 *         }
 *       );
 *     });
 */
@Injectable()
export class BiaSignalRService {
  /**
   * Object managing the SignalR connection.
   * You can access it through the 'getHubConnection()' method, so that every component/feature can add new event management,
   * without having to redefine every needed event and data structure in the domain.
   */
  private readonly hubConnection: HubConnection;

  /**
   * Retrieve the hubConnection instance.
   */
  public getHubConnection() {
    return this.hubConnection;
  }

  /**
   * Constructor.
   */
  public constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(environment.hubUrl)
      .build();

    this.configureConnection();
    this.startConnection();
  }

  /**
   * Configure the connection behavior.
   */
  private configureConnection(): void {
    this.hubConnection.onclose(async () => {
      console.log('%c [SignalRService] Hub connection closed. Try restarting it...', 'color: red; font-weight: bold');
      setTimeout(e => { this.startConnection(); }, 5000);
    });
  }

  /**
   * Start the SignalR connection.
   */
  private startConnection(): void {
    this.hubConnection
      .start()
      .then(() => {
        console.log('%c [SignalRService] Hub connection started', 'color: blue; font-weight: bold');
      })
      .catch((err: string) => {
        console.log('%c [SignalRService] Error while establishing connection, retrying...' + err,
          'color: red; font-weight: bold');
        setTimeout(e => { this.startConnection(); }, 5000);
      });
  }
}
