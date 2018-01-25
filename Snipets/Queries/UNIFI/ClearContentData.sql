-- Delete PubSub
Delete From Csg_Pubsub;

UPDATE CON_ITEM SET EXCLUSIVEEDITLOCK=0;

DELETE FROM CON_ITEMEXCLUSIVELOCK;

DECLARE

       commitCount  NUMBER  := 0;
       total  NUMBER  := 0;

       CURSOR del_record_cur IS
         Select Rowid
         From   Con_Wamwork;

     BEGIN
       FOR rec IN del_record_cur LOOP
         DELETE FROM Con_Wamwork
           WHERE rowid = rec.rowid;

         total := total + 1;
         commitCount := commitCount + 1;

         If (Commitcount >= 2) Then
          DBMS_OUTPUT.PUT_LINE('Do commit ' || commitCount || ' records from Con_Wamwork.');
           COMMIT;
           commitCount := 0;
         END IF;

       END LOOP;
       COMMIT;
       DBMS_OUTPUT.PUT_LINE('Deleted ' || total || ' records from Con_Wamwork.');
     End;
     /
     



-- delete all work areas
--Delete From Con_Wamwork;

-- delete all sample sets and analysis results
--Delete From Con_Item Where In ('32F7314B9ADE4441830A730893D2E7DB', 
--                               'D17340DC45FC47D2A2D1E4EFB727A40E')
--                               And Issubitem=1;
                               
--Delete From Con_Item Where Itemtypeid In ('B89E182F836745DA865C12D4EF60E97B', 
--                                          '1EE29011F12C475B97892E2DCF67903F', 
--                                         '50E20098FC5A4A998BA7FC211CE82848', 
--                                          '27FC931A0B584632855B6ED7A1825D08');

    
--Delete From Con_Subcatalog;

--Commit;
