HipCommit
=========

HipCommit is a simple SVN hook script for sending a detailed message to a HipChat chat-room when a commit takes place successfully.

Requirements
------------------

* A [HipChat room URL](https://www.hipchat.com/docs/apiv2/method/send_room_notification) (and auth token)

  `https://api.hipchat.com/v2/room/{YourRoom&AuthToekn}`
  
* An [Assembla Subversion repository URL](http://offers.assembla.com/free-subversion-hosting/)

  `https://subversion.assembla.com/svn/{YourSpaceName}/`
  
* An associated Assembla tickets URL:

  `https://www.assembla.com/spaces/{YourSpaceName}/tickets/`

Usage
--------

Add your application settings in the App.config file, build the project and then add the executable as an SVN hook script. For __svnadmin__ see [here](http://svnbook.red-bean.com/en/1.7/svn.reposadmin.create.html), for __TortoiseSVN__ see [here](http://tortoisesvn.net/docs/release/TortoiseSVN_en/tsvn-repository-hooks.html)
