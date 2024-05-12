# SK Event Bus

## Event buses

An event bus is a router that delivers events and receives them to zero or more destinations. Event buses are well-suited for routing events from many sources to many targets, with optional transformation of events prior to delivery to a target.

## Events

As its simplest, an event is a JSON object sent to an event bus.
In the context of event-driven architecture (EDA), an event often represents an indicator of a change in a resource or environment.

## Event handlers

The actual logic is contained in event-handler.
