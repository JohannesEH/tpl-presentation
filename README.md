# TPL Dataflow

```shell
dotnet add package System.Threading.Tasks.Dataflow
```

## Block Types

**Normal**
* ActionBlock
* BatchBlock
* BufferBlock
* TransformBlock
* TransformManyBlock

**Special**
* BetchedJoinBlock
* JoinBlock
* WriteOnceBlock


## Pizza Flow

```mermaid
graph LR;

TakeOrder -- OrderId --> CutDough
CutDough -- Pizza --> AddIngredients
AddIngredients -- Pizza --> Bake
Bake -- Pizza --> Batch
Batch -- PizzaBatch --> Deliver
```

## Topics

* Backpressure
* Parallelism
* 