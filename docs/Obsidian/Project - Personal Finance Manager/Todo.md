#### Accounts
- If accounts (both source and target) aren't recognized as any of the users accounts, the import of the row will fail with a "Unknown accounts" error. In the future I want to see about alternative ways to deal with this.
	- After import correct the incorrect transactions and assign them to a (new) account?

#### Transactions
- Add tags to create/update

#### Scalar
- Showing 404 when 500 gets returned?

#### General
- How to deal with SQL exceptions properly?

#### Testing
- Do current tests contain logic that can be extracted to the test utils project, and therefore be reused?

#### Logging
- Need to implement logging (seri/open telem)

#### Security
- Bij opslaan van imports op disk, deze encrypten met een auto-gen user-based key en die dan gebruiken voor de batch verwerking? Zo kan niemand die toegang tot de disk heeft de daadwerkelijke exports openen.



