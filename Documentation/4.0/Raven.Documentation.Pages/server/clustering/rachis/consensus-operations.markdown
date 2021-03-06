# Consensus Operations
---

{NOTE: }

* Any operation that is made at the _cluster level_ requires a consensus,  
  meaning it will either be accepted by the majority of the cluster nodes (n/2 + 1), or completely fail to register.  

* This operation is named **Raft Command** or Raft Log.  
  Once issued, it is propagated through [Rachis](../../../server/clustering/rachis/what-is-rachis) to all the nodes.  
  Only after the cluster's approval (majority of nodes ), it will _eventually_ be executed on all cluster nodes.  

* In this page:  
  * [Operations that require Consensus](../../../server/clustering/rachis/consensus-operations#operations-that-require-consensus)  
  * [Operations that do not require Consensus](../../../server/clustering/rachis/consensus-operations#operations-that-do-not-require-consensus)  
  * [Raft Commands Implementation Details](../../../server/clustering/rachis/consensus-operations#raft-commands-implementation-details)  
{NOTE/}

---

{PANEL: Operations that require Consensus}

Since getting a consensus is an expensive operation, it is limited to the following operations only:  

* Creating / Deleting a database  
* Adding / Removing node to / from a Database Group  
* Changing database settings  (e.g.   revisions configuraton , conflict resolving)
* Creating / Deleting Indexes (static and auto indexes)  
* Configuring the [Ongoing Tasks](../../../studio/database/tasks/ongoing-tasks/general-info)  

See [Implementation Details](../../../server/clustering/rachis/consensus-operations#implementation-details) below.  

{PANEL/}

{PANEL: Operations that do not require Consensus}

* It is important to understand that any document related operation **does not** require a consensus.  
  Any **document CRUD operation** or performing a **query on an _existing_ index** is executed against a _specific node_,  
  even in the case of a cluster partition.  

* Since RavenDB keeps documents synchronized by [Replication](../../../server/clustering/replication/replication), 
  any such operation is automatically replicated to all other nodes in the Database Group, 
  so documents are always available for _Read_, _Write_ and _Query_ even if there is no majority of nodes in the cluster.  
{PANEL/}

{PANEL: Raft Commands Implementation Details}

### Raft Index

* Every Raft command is assigned a **Raft Index**, which corresponds to the commands sequence execution order.  
  For example, an operation with the index 7 is executed only after _all_ operations with a smaller index have been executed.  

* If needed, a client with [Valid User](../../../server/security/authorization/security-clearance-and-permissions#user) privileges 
  can wait for a certain Raft command index to be executed on a specific cluster node.  
  This is done by issuing the following REST API call:  

  | Action | Method | URL |
  | - | - | - |
  | Wait for Raft Command | `GET` | /rachis/waitfor?index=index |

* The request will return after the corresponding Raft command was successfully applied -or-  
  a `timeout` is returned after `Cluster.OperationTimeoutInSec` has passed (default: 15 seconds).  

### Raft Command Events Sequence

* When a Raft command is sent, the following **sequence of events** occurs:  

    1. The client sends the command to a cluster node.  
    2. If the receiving node is not the Leader, it redirects the command to the Leader.  
    3. The Leader appends the command to its log and propagates the command to all other nodes.  
    4. If the Leader receives an acknowledgment from the majority of nodes, the command is actually executed.  
    5. If the command is executed at the Leader node, it is committed to the Leader Log, and notification is sent to other nodes.  
       Once the other nodes receive the notification, they execute the command as well.  
    6. If a Non-Leader node executes the command, it is added to the node log as well.  
    7. The client receives the Raft Index of the command issued, so it can be waited upon.  
{PANEL/}
