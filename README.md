# Short Confirmation Codes
Short confirmation code plugin for Sitecore Experience Commerce

This plugin provides six character confirmation codes (configurable), randomly
generated, and tracked in the commerce database to avoid conflicts.  Shorter codes
are easier to communicate over support calls and for some clients may be 
more convenient than the 25 character confirmations that are provided out of
the box.

## Technical notes

* As a precatution against codes being resused ("collisions"), the plugin creates tracking entities.
* The code checks for and creates tracking entities within a transaction, to ensure uniqueness.
* If a unique code is not generated in the allowed number of tries (default 3), a 32 character guid is returned.
* The default setting uses 26 characters, and a six character code, allowing for 308,915,776 possible codes.

## To Use

1. Add project to your soluton. 
1. Update NuGet references to appropriate version for your Sitecore Commerce install.
1. Add reference to your commerce engine project.
1. (Optional) Create policy JSON to modifty settings (allowed characters, number of tries, length of code).
1. (Optional) Modify Sharding policy to put confimration code entities into a separate table. By default, they will go
to CommerceEntities. 
